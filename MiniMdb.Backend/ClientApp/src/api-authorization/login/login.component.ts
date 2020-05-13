import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../authorize.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, throwError } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { catchError } from 'rxjs/operators';

// The main responsibility of this component is to handle the user's login process.
// This is the starting point for the login process. Any component that needs to authenticate
// a user can simply perform a redirect to this component with a returnUrl query parameter and
// let the component perform the login and return back to the return url.
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  formName = 'name';
  formPassword = 'password';
  public message = new BehaviorSubject<string>(null);

  constructor(
    private authorizeService: AuthorizeService,
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {
    this.form = this.formBuilder.group(
      {
        name: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(100)]],
        password: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(100)]],
      }, [Validators.required, Validators.length]
    );
}

  ngOnInit() {
  }


  async login() {
    const name = this.form.get(this.formName).value;
    const password = this.form.get(this.formPassword).value;
    this.authorizeService.login(name, password).pipe(catchError(e => {
      this.message.next('Invalid credentials');
      return throwError(e);
    })).subscribe(success => {
      if (success) {
        this.router.navigate(['/']);
      }
    });
  }

  get name() { return this.form.get(this.formName); }
  get password() { return this.form.get(this.formPassword); }
}
