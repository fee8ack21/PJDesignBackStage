import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '', loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule),
  },
  {
    path: 'administrator', loadChildren: () => import('./features/administrator/administrator.module').then(m => m.AdministratorModule),
  },
  {
    path: 'unit', loadChildren: () => import('./features/unit/unit.module').then(m => m.UnitModule),
  },
  {
    path: 'review', loadChildren: () => import('./features/review/review.module').then(m => m.ReviewModule),
  },
  {
    path: 'portfolio', loadChildren: () => import('./features/portfolio/portfolio.module').then(m => m.PortfolioModule),
  },
  {
    path: 'footer', loadChildren: () => import('./features/footer/footer.module').then(m => m.FooterModule),
  },
  {
    path: 'home', loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule),
  },
  {
    path: 'contact', loadChildren: () => import('./features/contact/contact.module').then(m => m.ContactModule),
  },
  {
    path: 'question', loadChildren: () => import('./features/question/question.module').then(m => m.QuestionModule),
  },
  {
    path: 'type1', loadChildren: () => import('./features/type1/type1.module').then(m => m.Type1Module),
  },
  {
    path: 'type2', loadChildren: () => import('./features/type2/type2.module').then(m => m.Type2Module),
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
