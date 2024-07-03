import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';
import { take } from 'rxjs/operators';
import { environment } from '@environments/environment';

@Injectable()
//{  providedIn: 'root', //Indica que esse provider deve ser resolvido na raiz do projeto, podendo ser utilizado
// em outros componentes} uma outra maneira de criar um injeção de dependência
export class EventoService {
  baseURL = environment.apiURL + 'api/eventos';
  tokenHeader = new HttpHeaders({'Authorization': 'Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIzIiwidW5pcXVlX25hbWUiOiJmaWxoYSIsIm5iZiI6MTcyMDAzNTc3MywiZXhwIjoxNzIwMTIyMTczLCJpYXQiOjE3MjAwMzU3NzN9.UpJLZ0dCnRMpUXry0irTBRzA8h5iSWS42trnrynewpdOrVSiuXTZAecvFNRxxTEd51UUbM2q-_X4GkcXLT3_8Q'});
  constructor(private http: HttpClient) {}

  public getEventos() : Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseURL, {headers: this.tokenHeader})
    .pipe(take(1));
  }

  /*um Observable é usado para lidar com operações assíncronas e fluxos de dados ao longo do tempo.
  Ele é parte da programação reativa e permite tratar múltiplos valores assíncronos.
  Observables são implementados usando a biblioteca RxJS e são amplamente utilizados para lidar com eventos
  de DOM, chamadas de API HTTP e outras operações assíncronas em um aplicativo Angular.*/

  public getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/tema/${tema}`)
    .pipe(take(1));
  }

  public getEventoById(id: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/${id}`)
    .pipe(take(1));
  }

  public post(evento : Evento): Observable<Evento> {
    return this.http.post<Evento>(this.baseURL , evento)
    .pipe(take(1));
  }

  public put(evento : Evento): Observable<Evento> {
    return this.http.put<Evento>(`${this.baseURL}/${evento.eventoId}`, evento)
    .pipe(take(1));
  }

  public deleteEvento(id: number): Observable<any>{
    return this.http.delete(`${this.baseURL}/${id}`)
    .pipe(take(1));
  }

  public postUpload(eventoId: number, file: File): Observable<Evento> {
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload);

    return this.http
    .post<Evento>(`${this.baseURL}/upload-image/${eventoId}`, formData)
    .pipe(take(1));
  }

}
