import { Component, OnInit } from '@angular/core';
import { LoginDTO } from 'src/app/interfaces/login-dto';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  hide = true;

  public user:LoginDTO = {
    username: '',
    password: ''
  };

  public response:any;

  constructor(private authenticationService:AuthenticationService) { }

  ngOnInit(): void {
  }

  loginUser(){
    console.log(this.user);
    this.authenticationService.loginUser(this.user).subscribe((res) => {
      this.response = res;
      console.log(this.response);
    });
    console.log(this.response);
  }
}
