import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule, AsyncPipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { EventService } from '../../../../core/services/event.service';
import { EventDetail } from '../../../../core/models/event.detail';
import { Observable, filter, tap } from 'rxjs';
import { CreateEvent } from '../../../../core/models/create.event';

@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.html',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AsyncPipe]
})
export class EventFormComponent implements OnInit {

  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private eventService = inject(EventService);

  eventForm!: FormGroup;
  isEditMode: boolean = false;
  eventId: number | null = null;
  isLoading: boolean = false;
  errorMessage: string | null = null;

  ngOnInit(): void {
    this.initForm();

    this.route.paramMap.pipe(
      filter(params => params.has('id')),
      tap(params => {
        this.eventId = Number(params.get('id'));
        this.isEditMode = true;
        this.loadEventData(this.eventId!);
      })
    ).subscribe();
  }

  initForm(): void {
    this.eventForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      description: [''],
      dateTime: ['', [Validators.required]], 
      location: ['', [Validators.required, Validators.maxLength(200)]],
      capacity: [null, [Validators.min(1)]], 
      isPublic: [true] 
    });
  }

  loadEventData(id: number): void {
    this.isLoading = true;
    this.eventService.getEventDetails(id).subscribe({
      next: (event: EventDetail) => {

        this.eventForm.patchValue({
          title: event.title,
          description: event.description,
          dateTime: this.formatDateForInput(event.dateTime),
          location: event.location,
          capacity: event.capacity,
          isPublic: event.isPublic
        });
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load event for editing.';
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  private formatDateForInput(date: Date | string): string {
    if (!date) return '';

    let dateString = date.toString();

    if (typeof date === 'string' && !dateString.endsWith('Z') && !dateString.includes('+')) {
      dateString += 'Z';
    }

    const d = new Date(dateString);

    if (isNaN(d.getTime())) {
      console.error("DEBUG: Date is Invalid. Check backend format:", date);
      this.errorMessage = 'Invalid date format received from server.';
      return '';
    }

    const offset = d.getTimezoneOffset() * 60000;
    const localISOTime = new Date(d.getTime() - offset).toISOString().slice(0, 16);

    return localISOTime;
  }

  onSubmit(): void {
    this.errorMessage = null;
    if (this.eventForm.invalid) {
      this.eventForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const formValue = this.eventForm.value;
    const payload: CreateEvent = {
      ...formValue,
      dateTime: new Date(formValue.dateTime)
    };

    const action$: Observable<any> = this.isEditMode
      ? this.eventService.updateEvent(this.eventId!, payload) 
      : this.eventService.createEvent(payload);            

    action$.subscribe({
      next: (res) => {
        this.isLoading = false; 

        let newId: number | null = null;

        if (this.isEditMode) {
          newId = this.eventId;
        } else if (res && typeof res.id === 'number') {

          newId = res.id;
        } else if (typeof res === 'number') {

          newId = res;
        }

        if (newId) {
          this.router.navigate(['/events', newId]); 
        } else {

          this.router.navigate(['/events']);
        }
      },
      error: (err) => {
        this.errorMessage = this.isEditMode ? 'Failed to update event.' : 'Failed to create event.';
        this.isLoading = false;
        console.error(err);
      }
    });
  }
  get f() { return this.eventForm.controls; }
}