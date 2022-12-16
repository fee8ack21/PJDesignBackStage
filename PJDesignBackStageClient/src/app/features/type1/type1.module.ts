import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { Type1RoutingModule } from './type1-routing.module';
import { Type1Component } from './type1.component';
import { SharedModule } from 'src/app/shared/modules/shared.module';


@NgModule({
  declarations: [
    Type1Component
  ],
  imports: [
    CommonModule,
    SharedModule,
    Type1RoutingModule
  ]
})
export class Type1Module { }
