import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginDTO } from 'src/app/interfaces/login-dto';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { CommonService } from 'src/app/services/common.service';

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
  public showError: string | boolean = false;

  constructor(private authenticationService:AuthenticationService, private router:Router, private commonService: CommonService) { }

  ngOnInit(): void {
  }

  loginUser(){
    if(!this.user.username && !this.user.password){
      this.showError = 'The username and password field are required';
    }
    else{
      if(!this.user.username){
        this.showError = 'The username field is required';
      }
      else{
        if(!this.user.password){
          this.showError = 'The password field is required';
        }
        else{
          this.showError = false;
          this.authenticationService.loginUser(this.user).subscribe(
            (res) => {
              this.authenticationService.saveLoggedUser(res);
              this.router.navigate(['home']);
              this.commonService.sendUpdate(res);
            }, error => {
              this.showError = error.error;
            }
          );
        }
      }
    }
  }
}
