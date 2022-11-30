import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from 'src/app/shared/components/layout/layout.component';
import { AuthGuard } from 'src/app/shared/guards/auth-guard';
import { AdministratorDetailComponent } from './administrator-detail/administrator-detail.component';
import { AdministratorListComponent } from './administrator-list/administrator-list.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: AdministratorListComponent,
      },
      {
        path: 'detail',
        component: AdministratorDetailComponent,
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdministratorRoutingModule { }
