using System;

namespace RoutingAlgorithmProject.Routing.Models
{
    public class AttributeDescription
    {
        /**
         * Constructor
         *
         * @param identifier
         *             Unique identifier
         * @param networkTableName
         *             Name of the network table that the attribute corresponds to
         * @param name
         *             Human readable name of the attribute
         * @param units
         *             Unit of the described attribute
         * @param dataType
         *             Data type of the described attribute
         * @param description
         *             Human readable description of the attribute
         * @param attributedType
         *             Indicator of what's being described
         *
         */
        protected AttributeDescription( int identifier,
                                        String         networkTableName,
                                        String         name,
                                        String         units,
                                        DataType       dataType,
                                        String         description,
                                        AttributedType attributedType)
        {
            this.identifier = identifier;
            this.networkTableName = networkTableName;
            this.name = name;
            this.units = units;
            this.dataType = dataType;
            this.description = description;
            this.attributedType = attributedType;
        }

        @Override
    public String toString()
        {
            return String.format("%d (%s, %s, %s, %s, %s, %s)",
                                 this.identifier,
                                 this.networkTableName,
                                 this.name,
                                 this.units,
                                 this.dataType.toString(),
                                 this.description,
                                 this.attributedType.toString());
        }

        /**
         * @return the identifier
         */
        public int getIdentifier()
        {
            return this.identifier;
        }

        /**
         * @return the networkTableName
         */
        public String getNetworkTableName()
        {
            return this.networkTableName;
        }

        /**
         * @return the name
         */
        public String getName()
        {
            return this.name;
        }

        /**
         * @return the name
         */
        public String getUnits()
        {
            return this.units;
        }

        /**
         * @return the data type
         */
        public DataType getDataType()
        {
            return this.dataType;
        }

        /**
         * @return the description
         */
        public String getDescription()
        {
            return this.description;
        }

        /**
         * @return the attributedType
         */
        public AttributedType getAttributedType()
        {
            return this.attributedType;
        }

        /**
         * Check that a value agrees with this description's data type
         *
         * @param value
         *             Any value
         * @return <tt>true</tt> if {@link Object#getClass()} agrees with the data
         *             type, otherwise <tt>false</tt>
         * @throws IllegalArgumentException
         *             if the value given is null
         */
        public <T> boolean dataTypeAgrees( T value)
        {
            if (value == null)
            {
                throw new IllegalArgumentException("Value may not be null.");
            }
            return this.dataTypeAgrees(value.getClass());
        }

        /**
         * Check that a {@link Class} agrees with this description's data type
         *
         * @param clazz
         *             Type class
         * @return <tt>true</tt> if this description's equivalent type class with
         *             the input type class
         */
        public boolean dataTypeAgrees( Class<?> clazz)
        {
            switch (this.dataType)
            {
                case Blob: return byte[].class.isAssignableFrom(clazz);
            case Integer: return Integer.class.isAssignableFrom(clazz);
            case Real:    return Double.class.isAssignableFrom(clazz) ||
                                 Float.class.isAssignableFrom(clazz);
            case Text:    return String.class.isAssignableFrom(clazz);

            default: throw new RuntimeException("Bad enum value for DataType");
    }
}

private  int identifier;
private  String         networkTableName;
    private  String         name;
    private  String         units;
    private  DataType       dataType;
    private  String         description;
    private  AttributedType attributedType;
}

}
