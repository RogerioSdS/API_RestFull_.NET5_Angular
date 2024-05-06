import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';

@Injectable()
//{  providedIn: 'root', //Indica que esse provider deve ser resolvido na raiz do projeto, podendo ser utilizado
// em outros componentes} uma outra maneira de criar um injeção de dependência
export class EventoService {
  baseURL = 'https://localhost:5001/api/eventos';
  constructor(private http: HttpClient) {}

  public getEventos() : Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseURL);
  }

  /*um Observable é usado para lidar com operações assíncronas e fluxos de dados ao longo do tempo.
  Ele é parte da programação reativa e permite tratar múltiplos valores assíncronos.
  Observables são implementados usando a biblioteca RxJS e são amplamente utilizados para lidar com eventos
  de DOM, chamadas de API HTTP e outras operações assíncronas em um aplicativo Angular.*/

  public getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/tema/${tema}`);
  }

  public getEventosById(id: number): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/tema/${id}`);
  }
}
