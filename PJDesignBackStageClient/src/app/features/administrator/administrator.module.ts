import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdministratorRoutingModule } from './administrator-routing.module';
import { AdministratorListComponent } from './administrator-list/administrator-list.component';
import { AdministratorDetailComponent } from './administrator-detail/administrator-detail.component';
import { SharedModule } from 'src/app/shared/modules/shared.module';
import { GroupDialogComponent } from './feature-shared/components/group-dialog/group-dialog.component';

@NgModule({
  declarations: [
    AdministratorListComponent,
    AdministratorDetailComponent,
    GroupDialogComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    AdministratorRoutingModule
  ]
})
export class AdministratorModule { }
