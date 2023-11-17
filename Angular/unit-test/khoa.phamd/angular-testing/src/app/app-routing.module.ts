import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from "./courses/home/home.component";
import {AboutComponent} from "./about/about.component";
import {CourseComponent} from "./courses/course/course.component";
import {courseResolver} from "./courses/services/course.resolver";
import { FirstDemoComponent } from './first-demo/first-demo.component';

const routes: Routes = [
    {
        path: "home",
        component: HomeComponent
    },
    { path: 'testing-service', loadChildren: () => import('./testing-service/testing-service.module').then(m => m.TestingServiceModule) },
    { path: 'testing-http-service', loadChildren: () => import('./testing-http-service/testing-http-service.module').then(m => m.TestingHttpServiceModule) },
    { path: 'testing-component', redirectTo: 'home' },
    { path: 'testing-directive', loadChildren: () => import('./testing-directive/testing-directive.module').then(m => m.TestingDirectiveModule) },
    { path: 'testing-pipe', loadChildren: () => import('./testing-pipe/testing-pipe.module').then(m => m.TestingPipeModule) },

    {
        path: "about",
        component: AboutComponent
    },
    {
        path: "first-demo",
        component: FirstDemoComponent
    },
    {
        path: 'courses/:id',
        component: CourseComponent,
        resolve: {
            course: courseResolver
        }
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
