import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReviewRoutingModule } from './review-routing.module';
import { ReviewListComponent } from './review-list/review-list.component';
import { SharedModule } from 'src/app/shared/modules/shared.module';

@NgModule({
  declarations: [
    ReviewListComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ReviewRoutingModule
  ]
})
export class ReviewModule { }
