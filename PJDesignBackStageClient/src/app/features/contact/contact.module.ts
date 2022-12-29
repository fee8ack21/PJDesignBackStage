import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContactRoutingModule } from './contact-routing.module';
import { ContactListComponent } from './contact-list/contact-list.component';
import { SharedModule } from 'src/app/shared/modules/shared.module';
import { ContactDialogComponent } from './feature-shared/components/contact-dialog/contact-dialog.component';

@NgModule({
  declarations: [
    ContactListComponent,
    ContactDialogComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ContactRoutingModule
  ]
})
export class ContactModule { }
