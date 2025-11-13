import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from "./shared/components/header/header";
import { AiAssistantComponent } from './shared/components/ai-assistant/ai-assistant';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Header, AiAssistantComponent],
  templateUrl: './app.html',
  providers: []
})

export class App {
  protected readonly title = signal('Client');
  isSidebarOpen = AiAssistantComponent.isOpen;
}
