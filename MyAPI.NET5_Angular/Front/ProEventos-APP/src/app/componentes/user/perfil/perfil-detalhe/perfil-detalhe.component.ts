import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import {
  AbstractControlOptions,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss'],
})
export class PerfilDetalheComponent implements OnInit {
  @Output() changeFormValue = new EventEmitter();
  userUpdate = {} as UserUpdate;
  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    public accountService: AccountService,
    public palestranteService: PalestranteService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {}

  get f(): any {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.validation();
    this.carregarUsuario();
    this.verificaForm();
  }

  /**
   * Verifica se o formulário sofreu alteração.
   * Se sim, emite um evento para o pai com o valor atual do formulário.
   */
  private verificaForm() : void{
    this.form.valueChanges.subscribe(() => {
      this.changeFormValue.emit({...this.form.value});
    });
  }

  carregarUsuario(): void {
    this.accountService
      .getuser()
      .subscribe(
        (userRetorno: UserUpdate) => {
          console.log(userRetorno);
          this.userUpdate = userRetorno;
          this.form.patchValue(this.userUpdate);
          this.toastr.success('Usuário carregado com sucesso!');
        },
        (error) => {
          this.toastr.error('Erro ao tentar carregar o usuário!');
          console.error(error);
          this.router.navigate(['/dashboard']);
        }
      )
      .add(() => this.spinner.hide());
  }

  private validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword'),
    };

    this.form = this.fb.group(
      {
        username: [''],
        imagemURL:[''],
        titulo: ['NaoInformado', Validators.required],
        primeiroNome: [
          '',
          [
            Validators.required,
            Validators.minLength(4),
            Validators.maxLength(50),
          ],
        ],
        ultimoNome: [
          '',
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(50),
          ],
        ],
        email: ['', [Validators.required, Validators.email]],
        phoneNumber: ['', Validators.required],
        descricao: [''],
        funcao: ['NaoInformado', Validators.required],
        //Verifica se o campo é nulo, se não for, ele passa o minimo de 6 caracteres
        password: ['', [Validators.minLength(6), Validators.nullValidator]],
        confirmePassword: ['', Validators.nullValidator],
      },
      formOptions
    );
  }

  onSubmit(): void {
    this.atualizarUsuario();
  }
  atualizarUsuario() {
    this.userUpdate = { ...this.form.value };
    this.spinner.show();

    if(this.f.funcao.value == 'Palestrante'){
      this.palestranteService.post().subscribe(
        () => {
          this.toastr.success('Função atualizada para participantes', 'Sucesso'),
          (error) => {
            this.toastr.error('A função palestrante não pode ser ativada', 'Erro');
            console.error(error);
          }
        }
      );
    }

    this.accountService
      .updateUser(this.userUpdate)
      .subscribe(
        () => {
          this.toastr.success('Usuário atualizado com sucesso!', 'Sucesso');
        },
        (error) => {
          this.toastr.error('Erro ao tentar atualizar o usuário!');
          console.error(error);
        }
      )
      .add(() => this.spinner.hide());
  }

  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
    this.form.get('titulo')?.setValue('');
  }
}
