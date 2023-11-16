import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AcomComponent } from './components/acom/acom.component';
import { DcomComponent } from './components/dcom/dcom.component';
import { DemoNgZoneComponent } from './components/demo-ngzone/demo-ngzone.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: true,
  imports: [CommonModule, AcomComponent, DcomComponent, DemoNgZoneComponent],
})
export class AppComponent {
  title = 'change-detection';

  appClick(): void {}
}
