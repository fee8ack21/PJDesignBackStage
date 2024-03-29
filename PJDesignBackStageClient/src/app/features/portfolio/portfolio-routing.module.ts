import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from 'src/app/shared/components/layout/layout.component';
import { AuthGuard } from 'src/app/shared/guards/auth-guard';
import { PortfolioDetailComponent } from './portfolio-detail/portfolio-detail.component';
import { PortfolioListComponent } from './portfolio-list/portfolio-list.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: PortfolioListComponent,
      },
      {
        path: 'detail',
        component: PortfolioDetailComponent,
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PortfolioRoutingModule { }
