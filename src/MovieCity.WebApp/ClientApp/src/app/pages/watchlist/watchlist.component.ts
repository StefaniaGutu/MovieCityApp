import { Component, OnInit } from '@angular/core';
import { MovieWithDetailsDto } from 'src/app/interfaces/movie-with-details-dto';
import { MovieService } from 'src/app/services/movie.service';

@Component({
  selector: 'app-watchlist',
  templateUrl: './watchlist.component.html',
  styleUrls: ['./watchlist.component.scss']
})
export class WatchlistComponent implements OnInit {
  public watchlist: MovieWithDetailsDto[] = [];
  public pageTitle: string = "Watchlist";
  public showNoResults: boolean = false;
  
  constructor(private movieService: MovieService) { }

  ngOnInit(): void {
    this.getWatchlist();
  }

  getWatchlist(){
    this.movieService.getWatchlist().subscribe((res) => {
      this.watchlist = res;
      this.showNoResults = true;
    })
  }
}
