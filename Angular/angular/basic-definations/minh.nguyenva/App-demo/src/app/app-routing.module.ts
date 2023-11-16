import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@core/keycloak/app.guard';
import { ContentComponent } from '@layout/containers';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  {
    path: '',
    component: ContentComponent,
    children: [
      {
        path: 'dashboard',
        loadChildren: () => import('@sct-modules-lib/src/lib/dashboard/dashboard.module').then((m) => m.DashboardModule),
      },
      {
        path: 'import',
        loadChildren: () => import('@sct-modules-lib/src/lib/import/import.module').then((m) => m.ImportModule),
      },
      {
        path: 'fuel-sales',
        loadChildren: () => import('@sct-modules-lib/src/lib/fuel-sales/fuel-sales.module').then((m) => m.FuelSalesModule),
      },
      {
        path: 'export',
        loadChildren: () => import('@sct-modules-lib/src/lib/export/export.module').then((m) => m.ExportModule),
      },
      {
        path: 'production',
        loadChildren: () => import('@sct-modules-lib/src/lib/production/production.module').then((m) => m.ProductionModule),
      },
      {
        path: 'configuration',
        loadChildren: () => import('@sct-modules-lib/src/lib/configuration/configuration.module').then((m) => m.ConfigurationModule),
      },
      {
        path: 'settings',
        loadChildren: () => import('@sct-modules-lib/src/lib/settings/settings.module').then((m) => m.SettingsModule),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { enableTracing: false })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
