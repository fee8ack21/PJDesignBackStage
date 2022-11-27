import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PortfolioRoutingModule } from './portfolio-routing.module';
import { PortfolioListComponent } from './portfolio-list/portfolio-list.component';
import { PortfolioDetailComponent } from './portfolio-detail/portfolio-detail.component';
import { SharedModule } from 'src/app/shared/modules/shared.module';


@NgModule({
  declarations: [
    PortfolioListComponent,
    PortfolioDetailComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    PortfolioRoutingModule
  ]
})
export class PortfolioModule { }
