import { Component, OnInit } from '@angular/core';
import { UserUpdate } from '@app/models/identity/UserUpdate';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss'],
})
export class PerfilComponent implements OnInit {

  usuario = {} as UserUpdate;

  constructor() {}

  ngOnInit() {}

  setFormValue(usuario:UserUpdate): void {
    this.usuario = usuario;
    }

  get f(): any {
    return '';
  }

}
