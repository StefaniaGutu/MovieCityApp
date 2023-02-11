import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ActorDetailsPageDto } from 'src/app/interfaces/actor-details-page-dto';
import { MovieService } from 'src/app/services/movie.service';

@Component({
  selector: 'app-actor-details',
  templateUrl: './actor-details.component.html',
  styleUrls: ['./actor-details.component.scss']
})
export class ActorDetailsComponent implements OnInit {
  actorId: string = '';
  actorDetails!: ActorDetailsPageDto;

  constructor(private movieService: MovieService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      this.actorId = params['id'];
    });

    this.getActorDetails();
  }

  getActorDetails(){
    this.movieService.getActorDetails(this.actorId).subscribe((res) => {
      this.actorDetails = res;
    })
  }
}
