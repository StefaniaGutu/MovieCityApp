import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageNotFoundComponent } from './error-pages/page-not-found/page-not-found.component';
import { AuthenticationGuard } from './guards/authentication.guard';
import { ActorDetailsComponent } from './pages/actor-details/actor-details.component';
import { HomeComponent } from './pages/home/home.component';
import { LikedMoviesComponent } from './pages/liked-movies/liked-movies.component';
import { LoginComponent } from './pages/login/login.component';
import { MovieDetailsComponent } from './pages/movie-details/movie-details.component';
import { MoviesAndSeriesComponent } from './pages/movies-and-series/movies-and-series.component';
import { RegisterComponent } from './pages/register/register.component';
import { UserProfileComponent } from './pages/user-profile/user-profile.component';
import { WatchedMoviesComponent } from './pages/watched-movies/watched-movies.component';
import { WatchlistComponent } from './pages/watchlist/watchlist.component';

const routes: Routes = [
  {
    path:'',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path:'home',
    component: HomeComponent
  },
  {
    path:'login',
    component: LoginComponent
  },
  {
    path:'register',
    component: RegisterComponent
  },
  {
    path:'moviesAndSeries',
    component: MoviesAndSeriesComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'movie/:id',
    component: MovieDetailsComponent
  },
  {
    path: 'actor/:id',
    component: ActorDetailsComponent
  },
  {
    path: 'profile/:id',
    component: UserProfileComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path:'likedMovies',
    component: LikedMoviesComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path:'watchedMovies',
    component: WatchedMoviesComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path:'watchlist',
    component: WatchlistComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: '**',
    component: PageNotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
