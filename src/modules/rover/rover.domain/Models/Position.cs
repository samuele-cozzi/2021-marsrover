//using rover.domain.AggregateModels.Abstracts;

using System;

namespace rover.domain.Models
{
    public class Position
    {
        public Coordinate Coordinate { get; set; }
        public FacingDirections FacingDirection { get; set; }

        public void MoveFarward(double angularStep)
        {
            if (this.FacingDirection == FacingDirections.N)
                Coordinate.Latitude += angularStep;
            if (this.FacingDirection == FacingDirections.S)
                Coordinate.Latitude -= angularStep;
            if (this.FacingDirection == FacingDirections.E)
                Coordinate.Longitude += angularStep;
            if (this.FacingDirection == FacingDirections.W)
                Coordinate.Longitude -= angularStep;

            this.Boundaries(angularStep);
        }

        public void MoveBackward(double angularStep)
        {
            if (this.FacingDirection == FacingDirections.N)
                Coordinate.Latitude -= angularStep;
            if (this.FacingDirection == FacingDirections.S)
                Coordinate.Latitude += angularStep;
            if (this.FacingDirection == FacingDirections.E)
                Coordinate.Longitude -= angularStep;
            if (this.FacingDirection == FacingDirections.W)
                Coordinate.Longitude += angularStep;

            this.Boundaries(angularStep);
        }

        private void Boundaries(double angularStep)
        {
            var circle = 360;
            Coordinate.Longitude = (Coordinate.Longitude >= -circle / 2 && Coordinate.Longitude <= circle / 2) 
                ? Coordinate.Longitude 
                : Math.Sign(Coordinate.Longitude) * (2 * angularStep) - Coordinate.Longitude;
            Coordinate.Latitude = (Coordinate.Latitude >= -circle / 4 && Coordinate.Latitude <= circle / 4) 
                ? Coordinate.Latitude 
                : throw new Exception();
        }
    }
}
