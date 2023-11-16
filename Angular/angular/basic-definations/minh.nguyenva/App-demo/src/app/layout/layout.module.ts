import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SideNavComponent, HeaderComponent } from './components';
import { ContentComponent } from './containers';
import { SharedModule } from '@sct-shared-lib';

const COMPONENTS = [SideNavComponent, HeaderComponent, ContentComponent];

@NgModule({
  declarations: [...COMPONENTS],
  imports: [CommonModule,RouterModule,SharedModule],
  exports: [...COMPONENTS],
})
export class LayoutModule {}
