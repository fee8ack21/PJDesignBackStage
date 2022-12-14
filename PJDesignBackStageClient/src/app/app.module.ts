import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpService } from './shared/services/http.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ValidatorService } from './shared/services/validator.service';
import { SharedModule } from './shared/modules/shared.module';
import { AuthModule } from './features/auth/auth.module';
import { AdministratorModule } from './features/administrator/administrator.module';
import { UnitModule } from './features/unit/unit.module';
import { ReviewModule } from './features/review/review.module';
import { PortfolioModule } from './features/portfolio/portfolio.module';
import { AuthService } from './shared/services/auth.service';
import { JwtInterceptor } from './shared/interceptors/jwt-interceptor';
import { ErrorInterceptor } from './shared/interceptors/error-interceptor';
import { SnackBarService } from './shared/services/snack-bar.service';
import { UnitService } from './shared/services/unit-service';
import { AuthGuard } from './shared/guards/auth-guard';
import { ProgressBarService } from './shared/services/progress-bar.service';
import { FooterModule } from './features/footer/footer.module';
import { ContactModule } from './features/contact/contact.module';
import { QuestionModule } from './features/question/question.module';
import { HomeModule } from './features/home/home.module';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    SharedModule,
    AuthModule,
    AdministratorModule,
    UnitModule,
    ReviewModule,
    PortfolioModule,
    FooterModule,
    ContactModule,
    QuestionModule,
    HomeModule,
  ],
  providers: [
    HttpService,
    ValidatorService,
    AuthService,
    SnackBarService,
    UnitService,
    AuthGuard,
    ProgressBarService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
