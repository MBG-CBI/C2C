import {Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { Error400Component } from './errors';

const routes: Routes = [
    { path: '400', component: Error400Component},
    { path: 'home', component: HomeComponent },
    { path: '', redirectTo: '/home', pathMatch: 'full' }
];

export const RoutingConfig = RouterModule.forRoot(routes);
