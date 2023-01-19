using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTP_20230119_Minesweeper
{
    /// <summary>
    /// Represents the state of a button which represents a mine. 
    /// </summary>
   public class MineButtonProps
   {
        #region Member Variables
        protected Guid mButtonID;
        protected EventHandler mHitMine;
        protected bool mIsToggled;
        protected MineButtonProps[] mNeighBours;
        #endregion

        #region Properties
        /// <summary>
        /// Does this button represent a mine?
        /// </summary>
        public bool IsMine { get; internal set; }
        /// <summary>
        /// Was the button toggled?
        /// </summary>
        public bool IsToggled 
        { 
            get => mIsToggled; 
            internal set 
            {
                mIsToggled = value;
                OnHitMine(this, null); 
            }
        }
        /// <summary>
        /// Represents the neighbours of a mine cell.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when the value does not have 8 elements.</exception>
        /// <exception cref="System.NullReferenceException">Thrown when the variable is null.</exception>
        public MineButtonProps[] Neighbours
        {
            get => mNeighBours;
            set
            {
                if (value.Length != 8)
                    throw new ArgumentException(message: "Expected 8 elements for the mine's neighbours");

                mNeighBours = value;

            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Fired when the mine was hit.
        /// </summary>
        public event EventHandler HitMine;
        #endregion

        #region Methods
        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="MineButtonProps"/> object.
        /// </summary>
        public MineButtonProps():this(false) 
        {
            byte[] nowBytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
            byte[] randomHash = new byte[8];
            Random prng = new Random(DateTime.UtcNow.Ticks.GetHashCode());
            prng.NextBytes(randomHash);

            byte[] unionHash = new byte[16];
            Array.Copy(randomHash, 0, unionHash, 0, 8);
            Array.Copy(randomHash, 0, unionHash, 8, 8);
            mButtonID = new Guid(unionHash);

            this.HitMine += OnHitMine;
        }

        static MineButtonProps()
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="MineButtonProps"/> object.
        /// </summary>
        /// <param name="isMine">Is this button a mine? Defaults to <see cref="false"/>.</param>
        public MineButtonProps(bool isMine)
        {
            byte[] nowBytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
            byte[] randomHash = new byte[8];
            Random prng = new Random(DateTime.UtcNow.Ticks.GetHashCode());
            prng.NextBytes(randomHash);

            byte[] unionHash = new byte[16];
            Array.Copy(randomHash, 0, unionHash, 0, 8);
            Array.Copy(randomHash, 0, unionHash, 8, 8);
            mButtonID = new Guid(unionHash);

            this.IsMine = isMine;

            this.HitMine += OnHitMine;
        }
        #endregion

        /// <summary>
        /// Toggles the state of this mine.
        /// </summary>
        public void Toggle() => IsToggled = !IsToggled;

 

        /// <summary>
        /// Generates a <see cref="Button"/> with these properties.
        /// </summary>
        /// <param name="size">The size of the <see cref="Button"/></param>
        /// <param name="pos">The location of the <see cref="Button"/></param>
        /// <returns>A <see cref="Button"/> with the specified properties, where its <see cref="Control.Tag"/> is set to this <see cref="MineButtonProps"/>.</returns>
        public Button GenerateButton(Size size, Point pos)
        {
            Button btn = new Button();
            byte[] idBytes = mButtonID.ToByteArray();
            btn.Size = size;
            btn.Location = pos;
            btn.Text = "";
            btn.Name = $"mine";
            foreach (var b in idBytes)
            {
                btn.Name += b.ToString("X4");
            }

            btn.Tag = this;

            btn.Click += (object sender, EventArgs e) => {
                this.HitMine(sender, e);
                this.Toggle();
            };

            this.HitMine += (object _sender, EventArgs _) => {
                Debug.WriteLine($"[USER-DEBUG]: {btn.Name} was clicked. It is{(this.IsMine ? "" : "n't")} a mine.");

                Button self = (Button)_sender;

                int closeMineCount = 0;

                foreach (var nei in mNeighBours)
                {
                    if (nei?.IsMine ?? false)
                        closeMineCount++;
                }

                self.Text = closeMineCount.ToString();

                if (this.IsMine)
                {
                    self.BackColor = Color.Red;

                }
            };

            return btn;
        }

        public override string ToString()
        {
            string props = "{";
            props += $" id = {mButtonID};";
            props += $" isMine = {IsMine};";
            props += $" isToggled = {IsToggled};";
            int closeMineCount = 0;

            foreach (var nei in mNeighBours)
            {
                if (nei?.IsMine ?? false)
                    closeMineCount++;
            }

            props += $" $neighbourMineCount = {closeMineCount};";
            //props += $" neighbours = [";

            //foreach (var nei in mNeighBours)
            //{
            //    props += $"  {nei}";
            //}

            //props += "];";
            props += "}";
            return props;
        }
        /// <summary>
        /// Fires a <see cref="MineButtonProps.HitMine"/> event.
        /// </summary>
        /// <param name="self">This object to fie the event.</param>
        /// <param name="p">Mandatory parameter for <see cref="System.EventHandler"/></param>
        private void OnHitMine(object self, EventArgs p)
        {

        }
        #endregion
    }
}
