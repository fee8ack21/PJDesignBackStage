import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdministratorRoutingModule } from './administrator-routing.module';
import { AdministratorListComponent } from './administrator-list/administrator-list.component';
import { AdministratorDetailComponent } from './administrator-detail/administrator-detail.component';
import { SharedModule } from 'src/app/shared/modules/shared.module';


@NgModule({
  declarations: [
    AdministratorListComponent,
    AdministratorDetailComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    AdministratorRoutingModule
  ]
})
export class AdministratorModule { }
