import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from "./courses/home/home.component";
import {AboutComponent} from "./about/about.component";
import { FirstDemoComponent } from './first-demo/first-demo.component';
import { TestingServiceComponent } from './testing-service/testing-service.component';
import { TestingHttpServiceComponent } from './testing-http-service/testing-http-service.component';
import { TestingDirectiveComponent } from './testing-directive/testing-directive.component';
import { TestingPipeComponent } from './testing-pipe/testing-pipe.component';

const routes: Routes = [
    {
        path: "home",
        component: HomeComponent
    },
    { path: 'testing-service', component: TestingServiceComponent },
    { path: 'testing-http-service', component: TestingHttpServiceComponent },
    { path: 'testing-component', redirectTo: 'home' },
    { path: 'testing-directive', component: TestingDirectiveComponent },
    { path: 'testing-pipe', component: TestingPipeComponent},

    {
        path: "about",
        component: AboutComponent
    },
    {
        path: "first-demo",
        component: FirstDemoComponent
    },
    { path: '', redirectTo: 'home', pathMatch: 'full' },

    // {
    //     path: "**",
    //     redirectTo: '/'
    // }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
