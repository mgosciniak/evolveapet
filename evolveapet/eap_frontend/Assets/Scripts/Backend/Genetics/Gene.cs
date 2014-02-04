﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace EvolveAPet
{
    public class Gene : AGene
    {
        private readonly Trait _trait;
        private readonly bool _type;
        private readonly char _symbol; // consider Dominant if uppercase
        private readonly int _data; // additional information needed for that 1 caracteristic
        // the students do not know this exist, this should be used only for our internal porpuses
        public bool Type { get { return _type; } }
        public char Symbol { get { return _symbol; } }
        public int Data { get { return _data; } }
        public Trait Trait { get { return _trait; } }
        public bool IsKnown { get; private set; }

        public Gene(char symbol, Trait trait, int data = 0)
        {

			//NEEDS CHANGED, THIS NEEDS TO WORK FOR EVERY GENE AND TRAIT INCLUDING COLOUR. DATA ARRAY MAY BE NEATER?
            _trait = trait;
            _symbol = symbol;
            _data = data;
            _type = char.IsUpper(symbol);
            IsKnown = false;
        }

        public void Discovered()
        {
            IsKnown = true;
        }


        public string Serialize()
        {
            throw new NotImplementedException("serialize gene");
        }

        public Gene(string serialGene)
        {

            // deserializing a gene

        }

    }
}
