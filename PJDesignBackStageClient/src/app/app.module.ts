import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpService } from './shared/services/http.service';
import { HttpClientModule } from '@angular/common/http';
import { ValidatorService } from './shared/services/validator.service';
import { SharedModule } from './shared/modules/shared.module';
import { AuthModule } from './features/auth/auth.module';
import { AdministratorModule } from './features/administrator/administrator.module';
import { UnitModule } from './features/unit/unit.module';

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
  ],
  providers: [
    HttpService,
    ValidatorService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
