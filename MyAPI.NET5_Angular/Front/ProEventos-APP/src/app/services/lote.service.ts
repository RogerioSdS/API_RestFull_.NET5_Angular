import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Lote } from '@app/models/Lote';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable(
  //Adicionei o root no app.module.ts, nos providers
)
export class LoteService {
  baseURL = 'https://localhost:5001/api/lotes';
  constructor(private http: HttpClient) {}

  public getLotesByEventoID(eventoId: number) : Observable<Lote[]> {
    return this.http.get<Lote[]>(`${this.baseURL}/${eventoId}`)
    .pipe(take(1));
  }

  /*um Observable é usado para lidar com operações assíncronas e fluxos de dados ao longo do tempo.
  Ele é parte da programação reativa e permite tratar múltiplos valores assíncronos.
  Observables são implementados usando a biblioteca RxJS e são amplamente utilizados para lidar com eventos
  de DOM, chamadas de API HTTP e outras operações assíncronas em um aplicativo Angular.*/

  public saveLote(eventoId: number, lotes : Lote[]): Observable<Lote[]> {
    return this.http.put<Lote[]>(`${this.baseURL}/${eventoId}`, lotes)
    .pipe(take(1));
  }

  public deleteEvento(eventoId : number, loteId: number): Observable<any>{
    return this.http.delete(`${this.baseURL}/${eventoId}/${loteId}`)
    .pipe(take(1));
  }

}
