import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Type2RoutingModule } from './type2-routing.module';
import { Type2ListComponent } from './type2-list/type2-list.component';
import { Type2DetailComponent } from './type2-detail/type2-detail.component';
import { SharedModule } from 'src/app/shared/modules/shared.module';

@NgModule({
  declarations: [
    Type2ListComponent,
    Type2DetailComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    Type2RoutingModule
  ]
})
export class Type2Module { }
