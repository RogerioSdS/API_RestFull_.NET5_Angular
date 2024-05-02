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

  getEventos() : Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseURL);
  }

  getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/tema/${tema}`);
  }

  getEventosById(id: number): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/tema/${id}`);
  }
}