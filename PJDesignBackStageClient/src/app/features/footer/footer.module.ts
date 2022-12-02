import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FooterRoutingModule } from './footer-routing.module';
import { FooterComponent } from './footer.component';
import { SharedModule } from 'src/app/shared/modules/shared.module';


@NgModule({
  declarations: [
    FooterComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    FooterRoutingModule
  ]
})
export class FooterModule { }
