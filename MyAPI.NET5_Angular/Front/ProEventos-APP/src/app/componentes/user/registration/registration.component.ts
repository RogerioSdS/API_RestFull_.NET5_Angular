import { ValidatorField } from './../../../helpers/ValidatorField';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { User } from '@app/models/identity/User';
import { AccountService } from '../../../services/account.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  user = {} as User;
  form!: FormGroup;

  constructor(private fb: FormBuilder,
              private router: Router,
              private toaster: ToastrService,
              private accountService: AccountService ) { }

  get f(): any { return this.form.controls; }

  ngOnInit(): void {
    this.validation();
  }

  private validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword')
    };

    this.form = this.fb.group({
      primeiroNome: ['', Validators.required],
      ultimoNome: ['', Validators.required],
      email: ['',
        [Validators.required, Validators.email]
      ],
      userName: ['', Validators.required],
      password: ['',
        [Validators.required, Validators.minLength(6)]
      ],
      confirmePassword: ['', Validators.required],
    }, formOptions);
  }
  register(): void {
    this.user = { ...this.form.value };
    // Copia os valores do formulário para a variável user.
    // O spread operator (...) é usado para spreadsar os valores do objeto form.value
    // na variável user.
    // Por exemplo, se form.value é {primeiroNome: 'John', ultimoNome: 'Doe'},
    // a variável user será {primeiroNome: 'John', ultimoNome: 'Doe'}.
    this.accountService.register(this.user).subscribe(
      () => this.router.navigateByUrl('/dashboard'),
      (error: any) => this.toaster.error(error.error, 'Erro')
    )
    //this.router.navigate(['/user/login']);
  }
}
