import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from 'src/app/shared/components/layout/layout.component';
import { AuthGuard } from 'src/app/shared/guards/auth-guard';
import { Type2DetailComponent } from './type2-detail/type2-detail.component';
import { Type2ListComponent } from './type2-list/type2-list.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: Type2ListComponent,
      },
      {
        path: 'detail',
        component: Type2DetailComponent,
      },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class Type2RoutingModule { }
