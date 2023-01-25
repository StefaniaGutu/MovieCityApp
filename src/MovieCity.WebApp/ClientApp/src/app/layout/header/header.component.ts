import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  public isUserLoggedIn = false;
  public userImage = '';
  public userUsername = '';

  private subscription: Subscription; 

  constructor(public authenticationService:AuthenticationService, private commonService: CommonService) { 
    this.subscription = this.commonService.getUpdate().subscribe
             (message => {
             this.isUserLoggedIn = true;
             //this.userImage = message.image;
             this.userImage = "../assets/no-profile-picture.png"
             this.userUsername = message.username;
             });
  }

  ngOnInit(): void {
    this.isUserLoggedIn = this.authenticationService.isUserLoggedIn();
    //this.userImage = this.authenticationService.getCurrentUserImage();
    //if(this.userImage == null || this.userImage === ''){
      this.userImage = "../assets/no-profile-picture.png"
    //}
    this.userUsername = this.authenticationService.getCurrentUserUsername();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  logoutUser(){
    this.authenticationService.logoutUser();
    this.isUserLoggedIn = false;
    this.userImage = '';
    this.userUsername = '';
  }
}
