import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UnitRoutingModule } from './unit-routing.module';
import { UnitListComponent } from './unit-list/unit-list.component';
import { SharedModule } from 'src/app/shared/modules/shared.module';


@NgModule({
  declarations: [
    UnitListComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    UnitRoutingModule
  ]
})
export class UnitModule { }
