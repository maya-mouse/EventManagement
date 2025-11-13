import { Component, inject, signal } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { CommonModule, AsyncPipe } from '@angular/common';
import { AiService } from '../../../core/services/ai.service'; // Припускаємо, що ти створив AiService
import { AuthService } from '../../../core/services/auth.service';
import { ChatMessage } from '../../../core/models/chat-message';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'app-ai-assistant',
  templateUrl: './ai-assistant.html',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AsyncPipe]
})
export class AiAssistantComponent {
  
  private aiService = inject(AiService);
  public authService = inject(AuthService); 
  private sanitizer = inject(DomSanitizer);

  public sanitizedAnswer: SafeHtml | null = null;
  public static isOpen = signal(false);
  public messages: ChatMessage[] = [];

  form = new FormGroup({
  question: new FormControl('', Validators.required)
});
  answer: string | null = null;
  isProcessing: boolean = false;
  
  isLoggedIn$ = this.authService.isLoggedIn$;

  constructor() {
    this.messages = this.aiService.loadHistory();
  }

  onSubmit(): void {
      if (this.form.invalid) return;
  const question = this.form.value.question!;
  this.messages = [...this.messages, { role: 'user', content: question }];
  this.aiService.saveHistory(this.messages);
  
  this.isProcessing = true;
  this.answer = null;

  this.aiService.ask(question).subscribe({
    next: res => {
      const assistantMessage: ChatMessage = { role: 'assistant', content: res };
      this.messages = [...this.messages, assistantMessage];
      this.aiService.saveHistory(this.messages);
      this.isProcessing = false;
    },
    error: err => {
        const errorMessage = 'AI API Error: Failed to get response. Check network/key.';
        const errorMsg: ChatMessage = { role: 'assistant', content: errorMessage };
        
        this.messages = [...this.messages, errorMsg];
        this.aiService.saveHistory(this.messages);
        
        this.isProcessing = false;
        console.error('API Error details:', err);
    }
  });
  }

  getSafeHtml(content: string): SafeHtml {
        return this.sanitizer.bypassSecurityTrustHtml(content.replace(/\*\*(.*?)\*\*/g, '<b>$1</b>')); 
    }
  closeSidebar() {
        AiAssistantComponent.isOpen.set(false);
    }
}