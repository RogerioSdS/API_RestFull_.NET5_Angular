import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; // Import the BrowserAnimationsModule

// HttpClientModule é um módulo do Angular que permite fazer requisições HTTP no backend
// através do serviço HttpClient. Ele é necessário para realizar requisições HTTP no projeto.
import { HttpClientModule } from '@angular/common/http';

import {CollapseModule} from 'ngx-bootstrap/collapse';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EventosComponent } from './eventos/eventos.component';
import { PalestrantesComponent } from './palestrantes/palestrantes.component';
import { NavComponent } from './nav/nav.component';

import { EventoService } from './services/evento.service';

import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';

@NgModule({
  declarations: [
    AppComponent,
    EventosComponent,
    PalestrantesComponent,
      NavComponent,
      DateTimeFormatPipe
   ],
    imports: [
      BrowserModule,
      AppRoutingModule,
      // HttpClientModule é um módulo do Angular que permite fazer requisições HTTP de forma fácil e intuitiva.
      // É usado para buscar dados de APIs ou servidores.
      HttpClientModule,
      BrowserAnimationsModule,
      CollapseModule.forRoot(),
      FormsModule,
      BsDropdownModule.forRoot(),
      TooltipModule.forRoot(),
      ModalModule.forRoot()

    ],
    providers: [
      EventoService //injecao de dependencia
    ],
    bootstrap: [AppComponent]
  })

export class AppModule { }
