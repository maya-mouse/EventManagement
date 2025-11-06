import { Routes } from '@angular/router';
import { EventsListComponent } from './modules/events/components/events-list/events-list';
import { Login } from './modules/auth/components/login/login';
import { Register } from './modules/auth/components/register/register';
import { EventDetailsComponent } from './modules/events/components/event-details/event-details';
import { MyEventsCalendarComponent } from './modules/events/components/my-events-calendar/my-events-calendar';
import { EventFormComponent } from './modules/events/components/event-form/event-form';
import { authGuard } from './core/guards/auth.guard';
import { ErrorPageComponent } from './shared/components/error-page/error-page';

export const routes: Routes = [
    { path: '', redirectTo: '/events', pathMatch: 'full' },
    { path: 'events/:id/edit', component: EventFormComponent, canActivate: [authGuard] },
    { path: 'create', component: EventFormComponent, canActivate: [authGuard] },
    { path: 'my-events', component: MyEventsCalendarComponent, canActivate: [authGuard] },
    { path: 'events', component: EventsListComponent },
    { path: 'events/:id', component: EventDetailsComponent, canActivate: [authGuard] },
    { path: 'register', component: Register },
    { path: 'login', component: Login },
    { path: '404', component: ErrorPageComponent },
    { path: '500', component: ErrorPageComponent},
    { path: '**',  redirectTo: '/404' }
];
