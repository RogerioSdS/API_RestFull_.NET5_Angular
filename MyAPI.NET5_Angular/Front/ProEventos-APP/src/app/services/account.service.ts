import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@app/models/identity/User';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { environment } from '@environments/environment';
import { Observable, ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Injectable()

export class AccountService {
  /**
   * currentUserSource é um ReplaySubject que armazena o último valor emitido pelo Observable.
   * Quando um novo assinante se inscreve nesse Observable, ele recebe o último valor emitido,
   * se houver, e qualquer valor subsequente que seja emitido.
   * No caso, o ReplaySubject armazena o último valor emitido, que é o usuário autenticado.
   * Isso é útil quando um novo componente precisa acessar o usuário autenticado.
   */
  private currentUserSource = new ReplaySubject<User>(1);
  /**
   * currentUser$ é um Observable, que esta apontado para o objeto ReplaySubject currentUserSource.
   * Isso permite que outros componentes se inscrevam nesse Observable para receber o último valor emitido,
   * que é o usuário autenticado.
   * Por exemplo, se um componente precisar acessar o usuário autenticado, ele pode se inscrever no currentUser$,
   * e receber o usuário autenticado atualizado sempre que ele for alterado.
   * Isso é útil, por exemplo, para exibir informações do usuário no cabeçalho da página.
   */
  public currentUser$ = this.currentUserSource.asObservable();


  baseURL = environment.apiURL + 'api/account/';
  constructor(private http: HttpClient) {}

  public login(model: any): Observable<void> {
    return this.http.post<User>(this.baseURL + 'login', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  public logout(): void{
    localStorage.removeItem('user');
    this.currentUserSource.next(null!);
    //this.currentUserSource.complete(); //concluiu as interrogações que deveriam ser passadas e completou o observable
  }

  public setCurrentUser(user: User): void {
    // Salva o usuário autenticado no armazenamento local (localStorage) como uma string JSON.
    // O localStorage é um armazenamento de dados que pode ser acessado por páginas web diferentes.
    // A função JSON.stringify() converte o objeto user em uma string JSON.
    localStorage.setItem('user', JSON.stringify(user));
    // Emite o usuário autenticado para o Observable currentUser$, que será recebido por outros componentes.
    this.currentUserSource.next(user);
  }

  getuser(): Observable<UserUpdate> {
    return this.http.get<UserUpdate>(this.baseURL + 'getUser').pipe(take(1));
  }

  updateUser(model: UserUpdate): Observable<void> {
    return this.http.put<UserUpdate>(this.baseURL + 'UpdateUser', model).pipe(
      take(1),
      map((user: UserUpdate) => {
          this.setCurrentUser(user);
        }
      )
    )
  }

  postUpload(file: File): Observable<UserUpdate> {
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload);

    return this.http
      .post<UserUpdate>(`${this.baseURL}upload-image`, formData)
      .pipe(take(1));
  }

  public register(model: any): Observable<void> {
    return this.http.post<User>(this.baseURL + 'register', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }
}
