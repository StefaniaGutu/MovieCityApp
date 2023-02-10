import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserProfileDto } from 'src/app/interfaces/user-profile-dto';
import { UserService } from 'src/app/services/user-service.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  constructor(private userService: UserService, private activatedRoute: ActivatedRoute) { }
  
  userId: string = '';
  userProfile!: UserProfileDto;

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      this.userId = params['id'];
    });

    this.getUserProfile();
  }

  getUserProfile(){
    this.userService.getUserProfile(this.userId).subscribe((res) => {
      this.userProfile = res;
    })
  }

}
