<?xml version="1.0" standalone="yes"?>
<xs:schema id="NewDataSet" xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
  <xs:element name="NewDataSet" msdata:IsDataSet="true" msdata:UseCurrentLocale="true">
    <xs:complexType>
      <xs:choice minOccurs="0" maxOccurs="unbounded">
        <xs:element name="tables">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="Name" type="xs:string" minOccurs="0" />
              <xs:element name="Caption" type="xs:string" minOccurs="0" />
              <xs:element name="Comment" type="xs:string" minOccurs="0" />
              <xs:element name="TablesId" msdata:AutoIncrement="true" type="xs:int" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="system">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="Name" type="xs:string" minOccurs="0" />
              <xs:element name="Value" type="xs:string" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="DeletedTables">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="Name" type="xs:string" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="SystemList">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="SystemListId" msdata:AutoIncrement="true" type="xs:int" minOccurs="0" />
              <xs:element name="SystemName" type="xs:string" minOccurs="0" />
              <xs:element name="ConnectionString" type="xs:string" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="oldtables">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="TablesId" type="xs:int" minOccurs="0" />
              <xs:element name="Name" type="xs:string" minOccurs="0" />
              <xs:element name="Caption" type="xs:string" minOccurs="0" />
              <xs:element name="Comment" type="xs:string" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="columns">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="Caption" type="xs:string" minOccurs="0" />
              <xs:element name="Name" type="xs:string" minOccurs="0" />
              <xs:element name="Comment" type="xs:string" minOccurs="0" />
              <xs:element name="ColumnsId" msdata:AutoIncrement="true" type="xs:int" minOccurs="0" />
              <xs:element name="DataType" type="xs:string" default="varchar" minOccurs="0" />
              <xs:element name="Length" type="xs:int" default="0" minOccurs="0" />
              <xs:element name="LengthForDecimal" type="xs:string" minOccurs="0" />
              <xs:element name="TablesId" type="xs:int" minOccurs="0" />
              <xs:element name="DefaultValue" type="xs:string" minOccurs="0" />
              <xs:element name="Enum" type="xs:string" minOccurs="0" />
              <xs:element name="CanNull" type="xs:boolean" default="true" minOccurs="0" />
              <xs:element name="ISPKID" type="xs:boolean" default="false" minOccurs="0" />
              <xs:element name="AutoIncrement" type="xs:boolean" default="false" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="oldcolumns">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="ColumnsId" type="xs:int" minOccurs="0" />
              <xs:element name="Caption" type="xs:string" minOccurs="0" />
              <xs:element name="Name" type="xs:string" minOccurs="0" />
              <xs:element name="Comment" type="xs:string" minOccurs="0" />
              <xs:element name="DataType" type="xs:string" default="varchar" minOccurs="0" />
              <xs:element name="Length" type="xs:int" default="0" minOccurs="0" />
              <xs:element name="LengthForDecimal" type="xs:string" minOccurs="0" />
              <xs:element name="TablesId" type="xs:int" minOccurs="0" />
              <xs:element name="DefaultValue" type="xs:string" minOccurs="0" />
              <xs:element name="Enum" type="xs:string" minOccurs="0" />
              <xs:element name="CanNull" type="xs:boolean" default="true" minOccurs="0" />
              <xs:element name="ISPKID" type="xs:boolean" default="false" minOccurs="0" />
              <xs:element name="AutoIncrement" type="xs:boolean" default="false" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="Relations">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="RelationsId" msdata:AutoIncrement="true" type="xs:int" minOccurs="0" />
              <xs:element name="TablesId_1" type="xs:int" minOccurs="0" />
              <xs:element name="TablesId_2" type="xs:int" minOccurs="0" />
              <xs:element name="ColumnsId_1" type="xs:int" minOccurs="0" />
              <xs:element name="ColumnsId_2" type="xs:int" minOccurs="0" />
              <xs:element name="Delete" type="xs:boolean" default="false" minOccurs="0" />
              <xs:element name="Update" type="xs:boolean" default="false" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="oldRelations">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="SqlString" type="xs:string" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="Menus">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="MenusId" msdata:AutoIncrement="true" type="xs:int" minOccurs="0" />
              <xs:element name="Caption" type="xs:string" minOccurs="0" />
              <xs:element name="parentid" type="xs:int" default="-1" minOccurs="0" />
              <xs:element name="Sort" type="xs:int" minOccurs="0" />
              <xs:element name="type" type="xs:int" default="1" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="Views">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="ID" msdata:AutoIncrement="true" type="xs:int" minOccurs="0" />
              <xs:element name="Name" type="xs:string" minOccurs="0" />
              <xs:element name="Sql" type="xs:string" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="RelationPos">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="RelationPosId" msdata:AutoIncrement="true" type="xs:int" minOccurs="0" />
              <xs:element name="RelationsId" type="xs:int" minOccurs="0" />
              <xs:element name="MenusId" type="xs:int" minOccurs="0" />
              <xs:element name="x1" type="xs:int" minOccurs="0" />
              <xs:element name="x2" type="xs:int" minOccurs="0" />
              <xs:element name="y2" type="xs:int" minOccurs="0" />
              <xs:element name="y1" type="xs:int" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="TablePos">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="TablePosId" msdata:AutoIncrement="true" type="xs:int" minOccurs="0" />
              <xs:element name="TablesId" type="xs:int" minOccurs="0" />
              <xs:element name="MenusId" type="xs:int" minOccurs="0" />
              <xs:element name="x" type="xs:int" minOccurs="0" />
              <xs:element name="y" type="xs:int" minOccurs="0" />
              <xs:element name="small" type="xs:boolean" default="false" minOccurs="0" />
              <xs:element name="deleted" type="xs:boolean" default="false" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="UniqueKey">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="UniqueKeyId" msdata:AutoIncrement="true" type="xs:int" minOccurs="0" />
              <xs:element name="TableId" type="xs:int" minOccurs="0" />
              <xs:element name="Name" type="xs:string" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="UniqueKeyItem">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="UniqueKeyItemId" msdata:AutoIncrement="true" type="xs:int" minOccurs="0" />
              <xs:element name="UniqueKeyId" type="xs:int" minOccurs="0" />
              <xs:element name="ColumnId" type="xs:int" minOccurs="0" />
              <xs:element name="Sort" type="xs:string" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="DeletedUniqueKey">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="Name" type="xs:string" minOccurs="0" />
              <xs:element name="TableId" type="xs:int" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="Trigger">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="TriggerId" msdata:AutoIncrement="true" type="xs:int" minOccurs="0" />
              <xs:element name="TableId" type="xs:int" minOccurs="0" />
              <xs:element name="Name" type="xs:string" minOccurs="0" />
              <xs:element name="Sql" type="xs:string" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="DeletedTrigger">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="Name" type="xs:string" minOccurs="0" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>