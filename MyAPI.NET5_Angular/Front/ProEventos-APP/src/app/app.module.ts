import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

// HttpClientModule é um módulo do Angular que permite fazer requisições HTTP no backend
// através do serviço HttpClient. Ele é necessário para realizar requisições HTTP no projeto.
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EventosComponent } from './eventos/eventos.component';
import { PalestrantesComponent } from './palestrantes/palestrantes.component';

@NgModule({
  declarations: [	
    AppComponent,
    EventosComponent,
      PalestrantesComponent
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    // HttpClientModule é um módulo do Angular que permite fazer requisições HTTP de forma fácil e intuitiva.
    // É usado para buscar dados de APIs ou servidores.
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})

export class AppModule { }
