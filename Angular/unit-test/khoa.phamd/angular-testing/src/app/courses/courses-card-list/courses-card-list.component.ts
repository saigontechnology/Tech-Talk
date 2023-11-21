import {Component, Input} from '@angular/core';
import {Course} from '../model/course';



@Component({
  selector: 'courses-card-list',
  templateUrl: './courses-card-list.component.html',
  styleUrls: ['./courses-card-list.component.css']
})
export class CoursesCardListComponent {

  @Input()
  courses: Course[];

}









