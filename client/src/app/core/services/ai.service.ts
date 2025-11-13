import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AskQuery } from '../models/ask-query';
import { ChatMessage } from '../models/chat-message';


@Injectable({
  providedIn: 'root'
})
export class AiService {
  private apiUrl = `${environment.apiUrl}/AiAssistant`;
  private http = inject(HttpClient);
  private chatHistoryKey = 'ai_chat_history';

  ask(question: string): Observable<string> {
    const payload: AskQuery = { question };
    return this.http.post(this.apiUrl, payload, { responseType: 'text' }); 
  }

  loadHistory(): ChatMessage[] {
    if (typeof sessionStorage !== 'undefined') {
      const history = sessionStorage.getItem(this.chatHistoryKey);
      return history ? JSON.parse(history) : [];
    }
    return [];
  }

  saveHistory(messages: ChatMessage[]): void {
    if (typeof sessionStorage !== 'undefined') {
      sessionStorage.setItem(this.chatHistoryKey, JSON.stringify(messages));
    }
  }
  
 
  clearHistory(): void {
      if (typeof sessionStorage !== 'undefined') {
          sessionStorage.removeItem(this.chatHistoryKey);
      }
  }
}