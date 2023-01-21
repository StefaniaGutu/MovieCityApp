import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  hide = true;
  hideConfirm = true;

  constructor(private formBuilder:FormBuilder, private authenticationService:AuthenticationService) { }

  public registerForm!:FormGroup;
  public response:any;

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      firstName:['', [Validators.required]],
      lastName:['', [Validators.required]],
      username:['', [Validators.required]],
      email:['', [Validators.required, Validators.email]],
      birthDate:['', [Validators.required]],
      password:['', [Validators.required]],
      confirmPassword:['', [Validators.required, this.passwordsAreEqual.bind(this)]]
    })
  }

  private passwordsAreEqual(fieldControl: FormControl) {
    if(this.registerForm == null)
      return null;
    return fieldControl.value === this.registerForm.controls["password"].value ? null : {
        NotEqual: true
    };
}

  registerUser(){
    console.log(this.registerForm.controls["email"].errors);
    console.log(this.registerForm);

    this.authenticationService.registerUser(this.registerForm.value).subscribe((res) => {
      this.response = res;
    })
  }
}
