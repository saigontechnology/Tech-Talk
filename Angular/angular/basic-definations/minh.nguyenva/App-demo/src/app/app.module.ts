import { LevelProgressComponent } from './../../projects/sct-shared-lib/src/lib/components/chart/level-progress/level-progress.component';
import { CoreModule } from './core/core.module';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { LayoutModule } from '@layout/layout.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, AppRoutingModule, LayoutModule, CoreModule],
  providers: [],
  bootstrap: [AppComponent],
  // entryComponents:[LevelProgressComponent] // when have Ivy we remove this step
})
export class AppModule {}
