import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UnitRoutingModule } from './unit-routing.module';
import { UnitComponent } from './unit.component';
import { SharedModule } from 'src/app/shared/modules/shared.module';
import { UnitDialogComponent } from './feature-shared/components/unit-dialog/unit-dialog.component';


@NgModule({
  declarations: [
    UnitComponent,
    UnitDialogComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    UnitRoutingModule
  ]
})
export class UnitModule { }
