import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from 'src/app/shared/components/layout/layout.component';
import { PortfolioDetailComponent } from './portfolio-detail/portfolio-detail.component';
import { PortfolioListComponent } from './portfolio-list/portfolio-list.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
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
