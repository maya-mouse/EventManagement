import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { CommonModule, AsyncPipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { EventService } from '../../../../core/services/event.service';
import { EventDetail } from '../../../../core/models/event.detail';
import { Observable, filter, tap, map, combineLatest, of, catchError, BehaviorSubject } from 'rxjs';
import { CreateEvent } from '../../../../core/models/create.event';
import { Tag } from '../../../../core/models/tag';

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
  


 private availableTagsSubject = new BehaviorSubject<Tag[]>([]);
public availableTags$ = this.availableTagsSubject.asObservable();
 public selectedTagNames: string[] = []; 
public newTagControl = new FormControl('');

  tagsControl: FormControl = new FormControl<string[]>([]);


 ngOnInit(): void {
 this.initForm();

  this.eventService.getAvailableTags().pipe(
    tap(tags => this.availableTagsSubject.next(tags))
  ).subscribe();


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
 isPublic: [true],

      tagNames: [[] as string[]] 
 });
 }

 loadEventData(id: number): void {
 this.isLoading = true;

    combineLatest([
        this.eventService.getEventDetails(id),
        this.availableTags$
    ]).pipe(
        tap(([event, availableTags]) => {
            this.eventForm.patchValue({
                title: event.title,
                description: event.description,
                dateTime: this.formatDateForInput(event.dateTime),
                location: event.location,
                capacity: event.capacity,
                isPublic: event.isPublic,
            });
            

            const currentTagNames = event.tags.map(t => t.name);
            this.eventForm.get('tagNames')?.setValue(currentTagNames);
            this.selectedTagNames = currentTagNames; 
            
            this.isLoading = false;
        }),
        catchError((err) => {
            this.errorMessage = 'Failed to load event for editing.';
            this.isLoading = false;
            return of(null);
        })
    ).subscribe();
  }


  onTagCheckboxChange(tagName: string, isChecked: boolean): void {
      const currentTags = this.eventForm.get('tagNames')!.value as string[];

      if (isChecked) {
          this.eventForm.get('tagNames')?.setValue([...currentTags, tagName]);
      } else {
          const updatedTags = currentTags.filter(t => t !== tagName);
          this.eventForm.get('tagNames')?.setValue(updatedTags);
      }

      this.selectedTagNames = this.eventForm.get('tagNames')!.value;
  }
  
 private formatDateForInput(date: Date | string): string {
 if (!date) return '';
    let dateString = date.toString();
    if (typeof date === 'string' && !dateString.endsWith('Z') && !dateString.includes('+')) {
 dateString += 'Z';
 }
 const d = new Date(dateString);
 if (isNaN(d.getTime())) {
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
 dateTime: new Date(formValue.dateTime),
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

addCustomTag(): void {
  const newTagName = (this.newTagControl.value || '').trim();
  if (!newTagName) return;

  const tagNamesControl = this.eventForm.get('tagNames');
  const currentTags = tagNamesControl!.value as string[];
  const availableTags = this.availableTagsSubject.value;

  if (currentTags.length >= 5) {
    this.errorMessage = 'Maximum 5 tags are allowed per event.';
    return;
  }


  const existsInAvailable = availableTags.some(
    t => t.name.toLowerCase() === newTagName.toLowerCase()
  );

  if (!existsInAvailable) {
    const newTag: Tag = { id: Date.now(), name: newTagName };
    this.availableTagsSubject.next([...availableTags, newTag]);
  }

  if (!currentTags.map(t => t.toLowerCase()).includes(newTagName.toLowerCase())) {
    tagNamesControl!.setValue([...currentTags, newTagName]);
  }

  tagNamesControl!.markAsDirty();
  this.newTagControl.setValue('');
  this.errorMessage = null;
}
 get f() { return this.eventForm.controls; }
}